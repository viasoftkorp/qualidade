import { VsAutocompleteModule } from '@viasoft/components';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

import { PRODUTOS_PROXY_URL } from './produto-autocomplete-select/produto-provider/tokens';
import { ProdutoProviderModule } from './produto-autocomplete-select/produto-provider/produto-autocomplete.module';
import { ProdutoAutocompleteSelectComponent } from './produto-autocomplete-select/produto-autocomplete-select.component';

import { NATUREZAS_PROXY_URL } from './natureza-autocomplete-select/natureza-provider/tokens';
import { NaturezaProviderModule } from './natureza-autocomplete-select/natureza-provider/natureza-provider.module';
import { NaturezaAutocompleteSelectComponent } from './natureza-autocomplete-select/natureza-autocomplete-select.component';

import { DEFEITOS_PROXY_URL } from './defeito-autocomplete-select/defeito-provider/tokens';
import { DefeitoAutocompleteSelectComponent } from './defeito-autocomplete-select/defeito-autocomplete-select.component';
import { DefeitoProviderModule } from './defeito-autocomplete-select/defeito-provider/defeito-provider.module';

import { RECURSOS_PROXY_URL } from './recurso-autocomplete-select/recurso-provider/tokens';
import { RecursoAutocompleteSelectComponent } from './recurso-autocomplete-select/recurso-autocomplete-select.component';
import { RecursoProviderModule } from './recurso-autocomplete-select/recurso-provider/recurso-autocomplete.module';

import { CAUSAS_PROXY_URL } from './causa-autocomplete-select/causa-provider/tokens';
import { CausaAutocompleteSelectComponent } from './causa-autocomplete-select/causa-autocomplete-select.component';
import { CausaProviderModule } from './causa-autocomplete-select/causa-provider/causa-provider.module';

import { SOLUCOES_PROXY_URL } from './solucao-autocomplete-select/solucao-provider/tokens';
import { SolucaoAutocompleteSelectComponent } from './solucao-autocomplete-select/solucao-autocomplete-select.component';
import { SolucaoProviderModule } from './solucao-autocomplete-select/solucao-provider/solucao-autocomplete.module';

import { ACOES_PREVENTIVAS_PROXY_URL } from './acao-preventiva-autocomplete-select/acao-preventiva-provider/tokens';
import { AcaoPreventivaAutocompleteSelectComponent } from './acao-preventiva-autocomplete-select/acao-preventiva-autocomplete-select.component';
import { AcaoPreventivaProviderModule } from './acao-preventiva-autocomplete-select/acao-preventiva-provider/acao-preventiva-autocomplete.module';

@NgModule({
  declarations: [
    ProdutoAutocompleteSelectComponent,
    NaturezaAutocompleteSelectComponent,
    DefeitoAutocompleteSelectComponent,
    RecursoAutocompleteSelectComponent,
    CausaAutocompleteSelectComponent,
    SolucaoAutocompleteSelectComponent,
    AcaoPreventivaAutocompleteSelectComponent,
  ],
  imports: [
    CommonModule,
    VsAutocompleteModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    ProdutoProviderModule,
    ProdutoAutocompleteSelectComponent,
    NaturezaProviderModule,
    NaturezaAutocompleteSelectComponent,
    DefeitoProviderModule,
    DefeitoAutocompleteSelectComponent,
    RecursoProviderModule,
    RecursoAutocompleteSelectComponent,
    CausaProviderModule,
    CausaAutocompleteSelectComponent,
    SolucaoProviderModule,
    SolucaoAutocompleteSelectComponent,
    AcaoPreventivaProviderModule,
    AcaoPreventivaAutocompleteSelectComponent,
  ],
  providers: [
    { provide: PRODUTOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/produtos' },
    { provide: NATUREZAS_PROXY_URL, useValue: 'qualidade/rnc/core/naturezas' },
    { provide: DEFEITOS_PROXY_URL, useValue: 'qualidade/rnc/core/defeitos' },
    { provide: RECURSOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/recursos' },
    { provide: CAUSAS_PROXY_URL, useValue: 'qualidade/rnc/core/causas' },
    { provide: SOLUCOES_PROXY_URL, useValue: 'qualidade/rnc/core/solucoes' },
    { provide: ACOES_PREVENTIVAS_PROXY_URL, useValue: 'qualidade/rnc/core/acoes-preventivas' },
  ]
})
export class ComponentsModule { }
