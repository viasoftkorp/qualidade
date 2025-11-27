import { IKeyTranslate } from '@viasoft/common';

export interface RncI18n extends IKeyTranslate {
  Rnc: {
    Navigation: {
      Causa: string,
      Configuracoes: string,
      Natureza: string,
      Solucao: string,
      AcaoPreventiva: string,
      Defeito: string,
      NaoConformidades: string
    }

    Permissions: {
      Administrator: string,
      SettingsCausas: string,
      SettingsCausasRead: string,
      SettingsCausasCreate: string,
      SettingsCausasDelete: string,
      SettingsCausasUpdate: string,
      SettingsNaturezas: string,
      SettingsNaturezasRead: string,
      SettingsNaturezasCreate: string,
      SettingsNaturezasDelete: string,
      SettingsNaturezasUpdate: string,
      SettingsSolucoes: string,
      SettingsSolucoesRead: string,
      SettingsSolucoesCreate: string,
      SettingsSolucoesDelete: string,
      SettingsSolucoesUpdate: string,
      SettingsAcoesPreventivas: string,
      SettingsAcoesPreventivasRead: string,
      SettingsAcoesPreventivasCreate: string,
      SettingsAcoesPreventivasDelete: string,
      SettingsAcoesPreventivasUpdate: string,
      SettingsDefeitos: string,
      SettingsDefeitosRead: string,
      SettingsDefeitosCreate: string,
      SettingsDefeitosDelete: string,
      SettingsDefeitosUpdate: string,
      NaoConformidades: string,
      NaoConformidadesRead: string,
      NaoConformidadesCreate: string,
      NaoConformidadesDelete: string,
      NaoConformidadesUpdate: string,
      PolicyManager: string,
      Settings:string
    }

    Salvar: string;
    Atencao: string;
    NaoPossuiNaturezaCadastrada: string;
  }
}
