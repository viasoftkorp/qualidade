import { VsAutocompleteModule } from '@viasoft/components';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SolucaoAutocompleteSelectComponent } from './solucao-autocomplete-select/solucao-autocomplete-select.component';
import { CausaAutocompleteSelectComponent } from './causa-autocomplete-select/causa-autocomplete-select.component';

import { RecursoAutocompleteSelectComponent } from './recurso-autocomplete-select/recurso-autocomplete-select.component';
import { ProdutoAutocompleteSelectComponent } from './produto-autocomplete-select/produto-autocomplete-select.component';
import { CAUSAS_PROXY_URL } from './causa-autocomplete-select/causa-provider/tokens';
import { CausaProviderModule } from './causa-autocomplete-select/causa-provider/causa-provider.module';
import { PRODUTOS_PROXY_URL } from './produto-autocomplete-select/produto-provider/tokens';
import { ProdutoProviderModule } from './produto-autocomplete-select/produto-autocomplete.module';

import { RECURSOS_PROXY_URL } from './recurso-autocomplete-select/recurso-provider/tokens';
import { SOLUCOES_PROXY_URL } from './solucao-autocomplete-select/solucao-provider/tokens';
import { RecursoProviderModule } from './recurso-autocomplete-select/recurso-provider/recurso-autocomplete.module';
import { SolucaoProviderModule } from './solucao-autocomplete-select/solucao-provider/solucao-autocomplete.module';
import { NATUREZAS_PROXY_URL } from './natureza-autocomplete-select/natureza-provider/tokens';
import { NaturezaProviderModule } from './natureza-autocomplete-select/natureza-provider/natureza-provider.module';
import { NaturezaAutocompleteSelectComponent } from './natureza-autocomplete-select/natureza-autocomplete-select.component';
import { DefeitoAutocompleteSelectComponent } from './defeito-autocomplete-select/defeito-autocomplete-select.component';
import { DefeitoProviderModule } from './defeito-autocomplete-select/defeito-provider/defeito-provider.module';
import { DEFEITOS_PROXY_URL } from './defeito-autocomplete-select/defeito-provider/tokens';
import { AcaoPreventivaAutocompleteSelectComponent }
  from './acao-preventiva-autocomplete-select/acao-preventiva-autocomplete-select.component';
import { ACOES_PREVENTIVAS_PROXY_URL } from './acao-preventiva-autocomplete-select/acao-preventiva-provider/tokens';
import { AcaoPreventivaProviderModule }
  from './acao-preventiva-autocomplete-select/acao-preventiva-provider/acao-preventiva-autocomplete.module';
import {
  OperacaoAutocompleteSelectComponent
} from './operacao-autocomplete-select/operacao-autocomplete-select.component';
import {
  OPERACOES_PROXY_URL
} from '../../../api-clients/Producao/Apontamento/Operacoes/Tokens';
import { OperacaoProxyService } from '@viasoft/rnc/api-clients/Producao/Apontamento/Operacoes/api/operacao-proxy.service';

@NgModule({
  declarations: [
    ProdutoAutocompleteSelectComponent,
    RecursoAutocompleteSelectComponent,
    CausaAutocompleteSelectComponent,
    SolucaoAutocompleteSelectComponent,
    NaturezaAutocompleteSelectComponent,
    DefeitoAutocompleteSelectComponent,
    AcaoPreventivaAutocompleteSelectComponent,
    OperacaoAutocompleteSelectComponent,
  ],
  imports: [
    CommonModule,
    VsAutocompleteModule,
    FormsModule,
    ReactiveFormsModule,
  ],
  exports: [
    ProdutoAutocompleteSelectComponent,
    RecursoAutocompleteSelectComponent,
    CausaAutocompleteSelectComponent,
    SolucaoAutocompleteSelectComponent,
    NaturezaAutocompleteSelectComponent,
    DefeitoAutocompleteSelectComponent,
    AcaoPreventivaAutocompleteSelectComponent,
    OperacaoAutocompleteSelectComponent,
    CausaProviderModule,
    ProdutoProviderModule,
    SolucaoProviderModule,
    RecursoProviderModule,
    NaturezaProviderModule,
    DefeitoProviderModule,
    AcaoPreventivaProviderModule,
  ],
  providers: [
    { provide: CAUSAS_PROXY_URL, useValue: 'qualidade/rnc/gateway/causas' },
    { provide: PRODUTOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/produtos' },
    { provide: SOLUCOES_PROXY_URL, useValue: 'qualidade/rnc/gateway/solucoes' },
    { provide: RECURSOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/recursos' },
    { provide: NATUREZAS_PROXY_URL, useValue: 'qualidade/rnc/gateway/naturezas' },
    { provide: DEFEITOS_PROXY_URL, useValue: 'qualidade/rnc/gateway/defeitos' },
    { provide: ACOES_PREVENTIVAS_PROXY_URL, useValue: 'qualidade/rnc/gateway/acoes-preventivas' },
    OperacaoProxyService, { provide: OPERACOES_PROXY_URL, useValue: 'qualidade/rnc/gateway/ordens-producao/operacoes' },
  ]
})
export class SharedModule { }
