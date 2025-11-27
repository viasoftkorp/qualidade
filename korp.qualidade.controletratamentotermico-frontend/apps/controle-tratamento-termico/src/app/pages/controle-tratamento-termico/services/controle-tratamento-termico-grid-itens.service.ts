import { effect, Injectable, signal, untracked } from "@angular/core";
import { VsGridComponent } from "@viasoft/components";

@Injectable()
export class ControleTratamentoTermicoGridItensService {
    public selectedLote = signal<string | null>(null);
    public gridPecasEl = signal<VsGridComponent>(undefined);

    constructor() {
        effect(() => {
            const grid = this.gridPecasEl();
            this.selectedLote();
            if (grid) {
                untracked(() => {
                    grid.options.refresh();
                });
            }
        });
    }
}