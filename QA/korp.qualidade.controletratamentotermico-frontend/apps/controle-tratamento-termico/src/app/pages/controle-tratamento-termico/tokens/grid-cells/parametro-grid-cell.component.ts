import { Component, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsGridGetInput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { PreCadastroCttGenericService } from "@viasoft/controle-tratamento-termico/app/services/pre-cadastro-ctt-generic.service";
import { PreCadastroCttGenericDtoOutput } from "@viasoft/controle-tratamento-termico/app/tokens";
import { from, map, of, tap } from "rxjs";

export interface IParametroGridCellData {
    codigoParametro: number;
    descricaoParametro: string;
}

@Component({
    selector: 'parametro-grid-cell',
    templateUrl: './parametro-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule],
    providers: [PreCadastroCttGenericService]
})
export class ParametroGridCellComponent extends VsTableCellCustomComponent<IParametroGridCellData, string> {
    public options = signal<PreCadastroCttGenericDtoOutput[]>([]);

    constructor(private preCadastroCttGenericService: PreCadastroCttGenericService) {
        super();
        preCadastroCttGenericService.baseUrlSuffix = 'parametros';
    }

    public getOptions = ((input: VsAutocompleteGetInput) => {
        const requestInput = new VsGridGetInput({
            filter: input.valueToFilter,
            skipCount: input.skipCount,
            maxResultCount: input.maxDropSize,
        });
        return from(this.preCadastroCttGenericService.buscarLista(requestInput)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: PreCadastroCttGenericDtoOutput) => ({
                        name: item.descricao,
                        value: item.codigo.toString()
                    })),
                    totalCount: result.totalCount
                } as VsAutocompleteOutput<string>;
            }),
        );
    }).bind(this);

    public getName = ((value: string) => of(this.data.descricaoParametro)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<string>): void {
        const selectedOptions = this.options().find(o => o.codigo.toString() === option.value);
        this.data.codigoParametro = selectedOptions.codigo;
        this.data.descricaoParametro = selectedOptions.descricao;
    }
}