import { ChangeDetectorRef, Component, effect, input, OnDestroy, OnInit, signal, ViewChild } from '@angular/core';
import { MessageService, VsAuthorizationService, VsSubscriptionManager } from '@viasoft/common';
import { VsGridOptions, VsGridComponent, IVsTableEditRowResult, VsGridNumberColumn, VsGridSimpleColumn, VsGridGetInput, VsGridGetResult, IVsTableEditRowErrorResult } from '@viasoft/components';
import { of, from, map, catchError } from 'rxjs';
import { PreCadastroMapValue, preCadastrosMap, PreCadastroType } from './pre-cadastro-ctt-generic.tokens';
import { HttpErrorResponse } from '@angular/common/http';
import { Policy } from '@viasoft/controle-tratamento-termico/app/authorization/policy';
import { PreCadastroCttGenericService } from '@viasoft/controle-tratamento-termico/app/services/pre-cadastro-ctt-generic.service';
import { getErrorMessage, PreCadastroCttGenericDtoOutput } from '@viasoft/controle-tratamento-termico/app/tokens';

@Component({
    selector: 'app-pre-cadastro-ctt-generic',
    templateUrl: './pre-cadastro-ctt-generic.component.html',
    styleUrls: ['./pre-cadastro-ctt-generic.component.scss']
})
export class PreCadastroCttGenericComponent implements OnDestroy, OnInit {
    public preCadastroType = input.required<PreCadastroType>();
    public id = input.required<string>();

    public isLoaded = signal<boolean>(false);
    private preCadastrosConfigValue: PreCadastroMapValue;

    private subs = new VsSubscriptionManager();

    public gridOptions: VsGridOptions<PreCadastroCttGenericDtoOutput>;
    public isCreatingRow = signal<boolean>(false);
    public isCreatingRowIndex = signal<number | undefined>(undefined);
    @ViewChild(VsGridComponent) gridEl: VsGridComponent;

    private policy = Policy;
    private preCadastroCttGenericRemover: boolean;
    private preCadastroCttGenericCriarEditar: boolean;

    constructor(
        private preCadastroCttGenericService: PreCadastroCttGenericService,
        private messageService: MessageService,
        private authorizationService: VsAuthorizationService,
        private changeDetectorRef: ChangeDetectorRef
    ) {
        effect(() => {
            if (!this.isLoaded()) return;
            this.subs.add('edit-grid', this.gridEl.editManagerService.isEditModeEnabledSuject.subscribe((isEditing) => {
                if (!isEditing && this.isCreatingRow() && this.isCreatingRowIndex() !== undefined && this.gridEl.editManagerService.rowToEditIndex() !== this.isCreatingRowIndex()) {
                    this.isCreatingRow.set(false);
                    this.gridEl.options.refresh();
                }
            }));
        }, { allowSignalWrites: true });
    }

    async ngOnInit(): Promise<void> {
        this.preCadastrosConfigValue = preCadastrosMap[this.preCadastroType()];
        this.preCadastroCttGenericService.baseUrlSuffix = this.preCadastrosConfigValue.suffixApiUrl;
        await this.getPermissions();
        this.configGridOptions();
        this.isLoaded.set(true);
        this.changeDetectorRef.detectChanges();
    }

    ngOnDestroy(): void {
        this.subs.clear();
    }

    private async getPermissions() {
        const policies: string[] = [
            this.policy[this.preCadastrosConfigValue.policies.criarEditar],
            this.policy[this.preCadastrosConfigValue.policies.excluir],
        ];
        const permissions = await this.authorizationService.isGrantedMap(policies);
        this.preCadastroCttGenericRemover = permissions.get(this.policy[this.preCadastrosConfigValue.policies.excluir]);
        this.preCadastroCttGenericCriarEditar = permissions.get(this.policy[this.preCadastrosConfigValue.policies.criarEditar]);
    }

    configGridOptions() {
        this.gridOptions = new VsGridOptions();
        this.gridOptions.id = this.id();
        this.gridOptions.enableVirtualScroll = true;
        this.gridOptions.rightActions = [
            {
                icon: 'plus',
                tooltip: 'PreCadastros.TratamentoTermicoGenerico.Adicionar',
                callback: () => {
                    this.isCreatingRow.set(true);
                    this.gridEl.options.refresh();
                },
                condition: () => this.preCadastroCttGenericCriarEditar
            }
        ];
        this.gridOptions.actions = [
            {
                callback: (rowIndex: number, data: PreCadastroCttGenericDtoOutput) => this.deletar(data.id),
                icon: 'trash-alt',
                tooltip: 'PreCadastros.TratamentoTermicoGenerico.Deletar',
                condition: () => this.preCadastroCttGenericRemover
            }
        ];
        this.gridOptions.editRowOptions = this.getEditRowOptions();
        this.gridOptions.columns = this.getGridColumns();
        this.gridOptions.get = (input) => this.buscarLista(input);
    }

    private getGridColumns() {
        return [
            new VsGridNumberColumn({
                field: 'codigo',
                headerName: 'PreCadastros.TratamentoTermicoGenerico.Codigo',
            }),
            new VsGridSimpleColumn({
                field: 'descricao',
                headerName: 'PreCadastros.TratamentoTermicoGenerico.Descricao',
            }),
        ];
    }

    private getEditRowOptions() {
        return {
            isAutoEditable: this.preCadastroCttGenericCriarEditar,
            shouldHideEditModeButtons: !this.preCadastroCttGenericCriarEditar,
            isCellEditable: (index, fieldName, data) => {
                return this.isCreatingRow() || fieldName !== 'codigo';
            },
            onRowEdit: (index, originalData, newData) => {
                const isDataInvalidMessage = this.validateData(newData);
                if (isDataInvalidMessage) {
                    return of({ success: false, errorMessage: isDataInvalidMessage } as IVsTableEditRowResult);
                }
                const isCreatingData = typeof originalData?.id === 'undefined';
                if (isCreatingData) {
                    // create data
                    return from(this.preCadastroCttGenericService.criar(newData)).pipe(map(result => {
                        return { success: true, shouldAutoRefreshGrid: true, updatedRowData: result } as IVsTableEditRowResult;
                    }), catchError((err: HttpErrorResponse) => {
                        return of({ success: false, errorMessage: err.error?.Message } as IVsTableEditRowResult);
                    }));
                } else {
                    // update data
                    newData.id = originalData.id;
                    return from(this.preCadastroCttGenericService.atualizar(newData)).pipe(map(result => {
                        return { success: true, shouldAutoRefreshGrid: true, updatedRowData: result } as IVsTableEditRowResult;
                    }), catchError((err: HttpErrorResponse) => {
                        return of({ success: false, errorMessage: err.error?.Message } as IVsTableEditRowResult);
                    }));
                }
            }
        };
    }

    private buscarLista(input: VsGridGetInput) {
        return from(this.preCadastroCttGenericService.buscarLista(input))
            .pipe(
                map(res => new VsGridGetResult(res.items)),
                map(result => {
                    if (!this.isCreatingRow()) {
                        return result;
                    }
                    // Add new line at the end so we can add lines too.
                    result.data = result.data.concat({
                        codigo: 0,
                        descricao: ''
                    } as PreCadastroCttGenericDtoOutput);

                    // TODO: we need 500ms so virtual scroll + edit + startEdit works properly
                    setTimeout(() => {
                        const index = result.data.length - 1;
                        this.isCreatingRowIndex.set(index);
                        this.gridEl.editManagerService.startEdit(index);
                    }, 500);

                    return result;
                })
            );
    }

    private validateData(data: any) {
        if (!data.descricao) { return 'PreCadastros.TratamentoTermicoGenerico.DescriptionRequired'; }
        return '';
    }

    private deletar(id: string): void {
        this.subs.add('msg-deletar', this.messageService
            .confirm('PreCadastros.TratamentoTermicoGenerico.ConfirmaDeletar')
            .subscribe((result: boolean) => {
                if (result) {
                    this.preCadastroCttGenericService
                        .deletar(id)
                        .then(() => {
                            this.gridOptions.refresh(true);
                        })
                        .catch((err: HttpErrorResponse) => {
                            this.messageService.info(getErrorMessage(err));
                        });
                }
            }));
    }
}