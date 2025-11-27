import { Component, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsGridGetInput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { PreCadastroCttGenericService } from "@viasoft/controle-tratamento-termico/app/services/pre-cadastro-ctt-generic.service";
import { PreCadastroCttGenericDtoOutput } from "@viasoft/controle-tratamento-termico/app/tokens";
import { from, map, of, tap } from "rxjs";

export interface ICalcoGridCellData {
    codigoCalco: number;
    descricaoCalco: string;
}

@Component({
    selector: 'calco-grid-cell',
    templateUrl: './calco-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule],
    providers: [PreCadastroCttGenericService]
})
export class CalcoGridCellComponent extends VsTableCellCustomComponent<ICalcoGridCellData, string> {
    public options = signal<PreCadastroCttGenericDtoOutput[]>([]);

    constructor(private preCadastroCttGenericService: PreCadastroCttGenericService) {
        super();
        preCadastroCttGenericService.baseUrlSuffix = 'calcos';
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

    public getName = ((value: string) => of(this.data.descricaoCalco)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<string>): void {
        const selectedOptions = this.options().find(o => o.codigo.toString() === option.value);
        this.data.codigoCalco = selectedOptions.codigo;
        this.data.descricaoCalco = selectedOptions.descricao;
    }
}