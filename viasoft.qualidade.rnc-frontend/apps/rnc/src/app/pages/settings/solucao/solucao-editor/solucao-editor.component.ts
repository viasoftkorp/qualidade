import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { EditorAction } from '@viasoft/rnc/app/tokens/consts/editor-action.enum';
import { SolucaoService } from '@viasoft/rnc/app/pages/settings/solucao/solucao.service';
import { SolucaoModel } from '@viasoft/rnc/api-clients/Solucoes';
import { ActivatedRoute, Router } from '@angular/router';
import { UUID } from 'angular2-uuid';
import { SolucaoFormControls } from '../solucao-form-controls';
import { finalize } from 'rxjs/operators';

@Component({
  selector: 'rnc-solucao-editor',
  templateUrl: './solucao-editor.component.html',
  styleUrls: ['./solucao-editor.component.scss']
})
export class SolucaoEditorComponent implements OnInit {
  public form: FormGroup = new FormGroup({});
  public formControls = SolucaoFormControls;
  public loaded = false;
  public editorAction:EditorAction;
  public processando = false;

  public get mostrarSubgruposSolucao():boolean {
    return this.editorAction === EditorAction.Update;
  }
  // eslint-disable-next-line @typescript-eslint/member-ordering
  private solucaoId: string;

  constructor(
    private formBuilder: FormBuilder,
    private service: SolucaoService,
    private route: ActivatedRoute,
    private router: Router,
  ) {
    this.formInit();
    this.formActionCheck();
  }

  ngOnInit(): void { // vazio
  }

  public canSave():boolean {
    return !this.processando && this.form.valid && this.form.dirty;
  }

  public save():void {
    if (this.canSave) {
      if (!this.canSave()) {
        return;
      }
      const solucao = this.form.getRawValue() as SolucaoModel;
      this.processando = true;
      if (this.editorAction === EditorAction.Create) {
        this.service.create(solucao)
          .pipe(finalize(() => {
            this.processando = false;
          }))
          .subscribe({
            next: () => {
              this.form.markAsPristine();
              this.router.navigate([`${solucao.id}`], { relativeTo: this.route.parent.parent })
                .then(() => this.formActionCheck());
            }
          });
      } else {
        this.service.update(solucao.id, solucao)
          .pipe(finalize(() => {
            this.processando = false;
          }))
          .subscribe({
            next: () => {
              this.form.markAsPristine();
            }
          });
      }
    }
  }

  private formInit() {
    this.form = this.formBuilder.group({});
    this.form.addControl(SolucaoFormControls.id, this.formBuilder.control(null, Validators.required));
    this.form.addControl(SolucaoFormControls.descricao,
      this.formBuilder.control(null, [Validators.required, Validators.maxLength(450)]));
    this.form.addControl(SolucaoFormControls.detalhamento, this.formBuilder.control(null));
    this.form.addControl(SolucaoFormControls.imediata, this.formBuilder.control(false));
    this.loaded = true;
  }

  private updateSolucao() {
    this.service.get(this.solucaoId).subscribe((solucao: SolucaoModel) => {
      this.populateForm(solucao);
      this.loaded = true;
    });
  }

  private populateForm(solucao: SolucaoModel) {
    this.form.get(SolucaoFormControls.id).setValue(solucao.id);
    this.form.get(SolucaoFormControls.descricao).setValue(solucao.descricao);
    this.form.get(SolucaoFormControls.detalhamento).setValue(solucao.detalhamento);
    this.form.get(SolucaoFormControls.imediata).setValue(solucao.imediata);
  }
  private formActionCheck() {
    if (this.route.snapshot.parent.paramMap.get('id') === 'new') {
      this.editorAction = EditorAction.Create;
      this.populateForm({
        id: UUID.UUID(),
      } as SolucaoModel);
    } else {
      this.editorAction = EditorAction.Update;
      this.solucaoId = this.route.snapshot.parent.paramMap.get('id');
      this.updateSolucao();
    }
  }
}

