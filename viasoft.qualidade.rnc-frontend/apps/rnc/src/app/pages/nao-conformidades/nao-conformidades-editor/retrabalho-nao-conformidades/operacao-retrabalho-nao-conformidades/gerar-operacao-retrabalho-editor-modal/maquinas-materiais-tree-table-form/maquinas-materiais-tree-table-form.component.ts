import { Component, OnInit } from '@angular/core';
import {
  VsDialog,
  VsTableHeaderAction,
  VsTreeTableAction,
  VsTreeTableColumn,
  VsTreeTableLoadEvent,
  VsTreeTableNode,
} from '@viasoft/components';
import { MessageService } from '@viasoft/common';
import { MaquinasEditorModalComponent } from './maquinas-editor-modal/maquinas-editor-modal.component';
import { MaquinaInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/maquina-input';
import { EditorModalData } from '@viasoft/rnc/app/tokens/consts/editor-modal-data';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { MatDialog } from '@angular/material/dialog';
import { MateriaisEditorModalComponent } from './materiais-editor-modal/materiais-editor-modal.component';
import { MaterialInput } from '@viasoft/rnc/api-clients/Nao-Conformidades/Operacao-Retrabalho/model/material-input';
import { FormArray, FormGroup, FormGroupDirective } from '@angular/forms';
import { GerarOperacaoRetrabalhoFormControls } from '../gerar-operacao-retrabalho-form-controls';
import { MaquinasFormControl } from './maquinas-editor-modal/maquinas-form-control';
import { MateriaisFormControl } from './materiais-editor-modal/materiais-form-control';

@Component({
  selector: 'rnc-maquinas-materiais-tree-table-form',
  templateUrl: './maquinas-materiais-tree-table-form.component.html',
  styleUrls: ['./maquinas-materiais-tree-table-form.component.scss'],
})
export class MaquinasMateriaisTreeTableFormComponent implements OnInit {
  public treeTableColumns: Array<VsTreeTableColumn>;
  public treeTableActions: Array<VsTreeTableAction>;
  public treeTableData: Array<VsTreeTableNode<any>> = [];
  public treeTableRightHeaderActions: Array<VsTableHeaderAction> = [];
  private form: FormArray;
  private maxResultCount = 100;
  private skipCount = 0;

  private get maquinas(): Array<MaquinaInput> {
    return this.form.getRawValue() as Array<MaquinaInput>;
  }

  public get totalCount(): number {
    return this.maquinas.length;
  }

  private get treeTableDataFromMaquinas(): Array<VsTreeTableNode<MaquinaInput>> {
    const output = this.maquinas.map((maquina: MaquinaInput) => {
      const materiaisTreeTableData = maquina.materiais.map((material: MaterialInput) => {
        return {
          data: material,
          leaf: true,
          expanded: false,
          children: [],
        } as VsTreeTableNode<MaterialInput>;
      });

      const hasMateriais = Boolean(maquina.materiais.length);

      return {
        data: maquina,
        leaf: !hasMateriais,
        expanded: hasMateriais,
        children: materiaisTreeTableData as unknown,
      } as VsTreeTableNode<MaquinaInput>;
    });

    return output;
  }

  constructor(private vsDialog: VsDialog, private matDialog: MatDialog, private formGroupDirective: FormGroupDirective,
    private messageService: MessageService) {}

  public ngOnInit(): void {
    this.setForm();
    this.createTreeTable();
  }

  public onLoadTreeTableHandle(event: VsTreeTableLoadEvent) {
    this.maxResultCount = event.maxResultCount;
    this.skipCount = event.skipCount;
    this.atualizarTreeTable();
  }

  public setForm() {
    const formularioGerarOperacaoRetrabalho = this.formGroupDirective.form;
    this.form = formularioGerarOperacaoRetrabalho.get(GerarOperacaoRetrabalhoFormControls.maquinas) as FormArray;
  }

  public abrirModalEdicao(entidade: MaquinaInput | MaterialInput): void {
    if (this.isMaquina(entidade)) {
      const maquina = entidade as MaquinaInput;
      this.abrirModalMaquina(maquina);
    } else {
      const material = entidade as MaterialInput;
      this.abrirModalMaterial(material.idMaquina, material);
    }
  }

  private isMaquina(entidade: MaterialInput | MaquinaInput): boolean {
    const entidadeTemPropriedadeMateriais = entidade.hasOwnProperty('materiais');
    return entidadeTemPropriedadeMateriais;
  }

  private atualizarTreeTable() {
    const maxPageRange = this.skipCount + this.maxResultCount;
    const dadosPaginados = this.treeTableDataFromMaquinas.slice(this.skipCount, maxPageRange);
    this.treeTableData = dadosPaginados;
  }

  private createTreeTable(): void {
    this.setHeaderActions();
    this.setTreeTableActions();
    this.setTreeTableColumns();
  }

  private setHeaderActions(): void {
    this.treeTableRightHeaderActions = [
      {
        icon: 'plus',
        tooltip: 'MaquinasMateriaisTreeTableForm.AdicionarMaquina',
        callback: () => this.abrirModalMaquina(null),
      },
    ];
  }

  private setTreeTableActions(): void {
    this.treeTableActions = [
      {
        icon: 'trash-alt',
        tooltip: 'MaquinasMateriaisTreeTableForm.Deletar',
        callback: (entidade: MaquinaInput | MaterialInput) => {
          this.messageService.confirm('MaquinasMateriaisTreeTableForm.DeletarConfirmMessage').subscribe((confirmed:boolean) => {
            if (confirmed) {
              if (this.isMaquina(entidade)) {
                this.deletarMaquina(entidade as MaquinaInput);
              } else {
                this.deletarMaterial(entidade as MaterialInput)
              }
              this.atualizarTreeTable();
            }
          });
        }
      } as VsTreeTableAction,
      {
        icon: 'plus',
        tooltip: 'MaquinasMateriaisTreeTableForm.AdicionarMaterial',
        callback: (entidade: MaquinaInput) => this.abrirModalMaterial(entidade.id, null),
        condition: (entidade: MaquinaInput | MaterialInput) => this.isMaquina(entidade),
      } as VsTreeTableAction
    ];
  }

  private getPosicaoMaquina(idMaquina: string):number {
    const posicao = this.maquinas.findIndex((e: MaquinaInput) => e.id === idMaquina)
    return posicao;
  }

  private deletarMaquina(maquina: MaquinaInput):void {
    const posicaoMaquina = this.getPosicaoMaquina(maquina.id);
    this.form.removeAt(posicaoMaquina);
  }

  private deletarMaterial(material: MaterialInput):void {
    const maquinaFormGroup = this.getMaquinaFormGroup(material.idMaquina);

    const materiaisFormArray = maquinaFormGroup.get(MaquinasFormControl.materiais) as FormArray;

    const posicaoMaterial = this.getPosicaoMaterial(material);

    materiaisFormArray.removeAt(posicaoMaterial);
  }

  private setTreeTableColumns(): void {
    this.treeTableColumns = [
      {
        field: 'descricao',
        headerName: 'MaquinasMateriaisTreeTableForm.Descricao',
      } as VsTreeTableColumn,
      {
        field: 'horas',
        headerName: 'MaquinasMateriaisTreeTableForm.Horas',
      } as VsTreeTableColumn,
      {
        field: 'minutos',
        headerName: 'MaquinasMateriaisTreeTableForm.Minutos',
      } as VsTreeTableColumn,
      {
        field: 'quantidade',
        headerName: 'MaquinasMateriaisTreeTableForm.Quantidade',
      } as VsTreeTableColumn,
      {
        field: 'detalhamento',
        headerName: 'MaquinasMateriaisTreeTableForm.Detalhamento',
      } as VsTreeTableColumn,
    ];
  }

  private getDialogConfig(action: EditorAction, data: unknown) {
    const dialogConfig = this.vsDialog.generateDialogConfig(new EditorModalData(action, data), {
      maxWidth: '30%'
    });

    return dialogConfig;
  }

  private getMaquinaFormGroup(idMaquina: string): FormGroup {
    const posicaoMaquina = this.getPosicaoMaquina(idMaquina);
    const maquinaFormGroup = this.form.get(posicaoMaquina.toString()) as FormGroup;
    return maquinaFormGroup;
  }

  private abrirModalMaquina(maquina: MaquinaInput | null) {
    const action = maquina ? EditorAction.Update : EditorAction.Create;

    let formGroup;
    if (action === EditorAction.Update) {
      formGroup = this.getMaquinaFormGroup(maquina.id);
    }

    const dialogConfig = this.getDialogConfig(action, formGroup);

    this.matDialog
      .open(MaquinasEditorModalComponent, dialogConfig)
      .afterClosed()
      .subscribe((maquinaInputFormGroup: FormGroup) => {
        const maquinaSalva = Boolean(maquinaInputFormGroup);

        if (!maquinaSalva && action === EditorAction.Create) {
          return;
        } else if (!maquinaSalva) {
          this.desatualizarMaquina(maquina);
        }

        if (action === EditorAction.Create) {
          this.inserirMaquina(maquinaInputFormGroup);
        }
        this.atualizarTreeTable();
      });
  }

  private inserirMaquina(maquinaInput: FormGroup) {
    this.form.push(maquinaInput);
    if (!this.form.dirty) {
      this.form.markAsDirty();
    }
  }

  private desatualizarMaquina(maquinaAntesAtualizacao: MaquinaInput) {
    const maquinaFormGroup = this.getMaquinaFormGroup(maquinaAntesAtualizacao.id);
    maquinaFormGroup.patchValue(maquinaAntesAtualizacao);
  }

  private getPosicaoMaterial(material: MaterialInput) {
    const maquinaFormGroup = this.getMaquinaFormGroup(material.idMaquina);

    const materiais = maquinaFormGroup.get(MaquinasFormControl.materiais).value as Array<MaterialInput>;

    const posicao = materiais.findIndex((e: MaterialInput) => e.id === material.id);
    return posicao;
  }

  private getMaterialFormGroup(material: MaterialInput): FormGroup {
    const posicaoMaterial = this.getPosicaoMaterial(material);

    const maquinaFormGroup = this.getMaquinaFormGroup(material.idMaquina);

    const materialFormGroup = maquinaFormGroup.get(MaquinasFormControl.materiais).get(posicaoMaterial.toString()) as FormGroup;
    return materialFormGroup;
  }

  private abrirModalMaterial(idMaquina: string, material: MaterialInput | null) {
    const action = material ? EditorAction.Update : EditorAction.Create;

    let formGroup;
    if (action === EditorAction.Update) {
      formGroup = this.getMaterialFormGroup(material);
    }

    const dialogConfig = this.getDialogConfig(action, { idMaquina, material: formGroup });

    this.matDialog
      .open(MateriaisEditorModalComponent, dialogConfig)
      .afterClosed()
      .subscribe((materialInput: FormGroup) => {
        const materialSalvo = Boolean(materialInput);

        if (!materialSalvo && action === EditorAction.Create) {
          return;
        } else if (!materialSalvo) {
          this.desatualizarMaterial(material);
        }

        if (action === EditorAction.Create) {
          this.inserirMaterial(materialInput);
        }

        this.atualizarTreeTable();
      });
  }

  private inserirMaterial(materialInput: FormGroup) {
    const idMaquina = materialInput.get(MateriaisFormControl.idMaquina).value as string;

    const maquinaFormGroup = this.getMaquinaFormGroup(idMaquina);

    const materiaisMaquinaFormArray = maquinaFormGroup.get(MaquinasFormControl.materiais) as FormArray;

    materiaisMaquinaFormArray.push(materialInput);
  }
  private desatualizarMaterial(materialAntesModificacao: MaterialInput) {
    const materialFormGroup = this.getMaterialFormGroup(materialAntesModificacao);

    materialFormGroup.patchValue(materialAntesModificacao);
  }
}
