import { NaoConformidadePolicies } from './nao-conformidade-policies';
import { RetrabalhoNaoConformidadePolicies } from './retrabalho-nao-comformidade-policies';
import { SettingsPolicies } from './settings-policies';

export class Policies extends SettingsPolicies {
  public static settingsPolicies = SettingsPolicies;
  public static NaoConformidadePolicies = NaoConformidadePolicies;
  public static retrabalhoNaoConformidadePolicies = RetrabalhoNaoConformidadePolicies;
}
