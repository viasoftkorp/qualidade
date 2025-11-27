import { Component, inject, signal } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { VsAutocompleteGetInput, VsAutocompleteModule, VsAutocompleteOption, VsAutocompleteOutput, VsGridGetInput, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";
import { from, map, of, tap } from "rxjs";
import { ControleTratamentoTermicoGridItensService } from "../../services/controle-tratamento-termico-grid-itens.service";
import { TratamentoTermicoApiService } from "@viasoft/controle-tratamento-termico/app/services/tratamento-termico-api.service";

export interface ILoteGridCellData {
    lote: string;
}

@Component({
    selector: 'lote-grid-cell',
    templateUrl: './lote-grid-cell.component.html',
    styles: 'vs-autocomplete-select { flex: auto; }',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsAutocompleteModule]
})
export class LoteGridCellComponent extends VsTableCellCustomComponent<ILoteGridCellData, string> {
    public options = signal<string[]>([]);

    public tratamentoTermicoService = inject(TratamentoTermicoApiService);
    public gridItensService = inject(ControleTratamentoTermicoGridItensService);

    public getLotes = ((input: VsAutocompleteGetInput) => {
        return from(this.tratamentoTermicoService.buscarListaLote(input)).pipe(
            tap((result) => {
                this.options.set(result?.items ?? []);
            }),
            map((result) => {
                return {
                    items: (result.items ?? []).map((item: string) => ({
                        name: item,
                        value: item
                    })),
                    totalCount: result.totalCount
                } as VsAutocompleteOutput<string>;
            }),
        );
    }).bind(this);

    public getLoteName = ((value: number) => of(this.data.lote)).bind(this);

    public onOptionSelected(option: VsAutocompleteOption<string>): void {
        this.data.lote = option.value;
        this.gridItensService.selectedLote.set(option.value);
    }
}