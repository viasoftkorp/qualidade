import { Observable, of } from 'rxjs';
import {
  VsCompaniesDetail, VsCompanyDetails, VsCompanyProviderService
} from '@viasoft/http';

export class MockCompanies extends VsCompanyProviderService {
  public getCompanies(): Observable<VsCompaniesDetail> {
    const companiesDatail = { } as VsCompaniesDetail;
    companiesDatail.companies = [
      {
        tenantId: '1',
        id: '6B1FA7AB-D290-4A75-96BD-7E561AA7BA84',
        legacyCompanyId: 1,
        cnpj: '00000000000',
        stateRegistration: 'RN',
        companyName: 'Exemplo',
        tradingName: 'Exemplo',
        phone: '11111111',
        email: '111@hotmail.com',
      } as VsCompanyDetails,
      {
        tenantId: '2',
        id: 'B89150D8-9603-481D-AAC4-1C26C4544F56',
        legacyCompanyId: 2,
        cnpj: '222222222',
        stateRegistration: 'RN',
        companyName: 'Exemplo2',
        tradingName: 'Exemplo2',
        phone: '22222222',
        email: '222@hotmail.com',
      } as VsCompanyDetails,
      {
        tenantId: '3',
        id: '7757C843-FB73-4651-9D9F-7E3996301601',
        legacyCompanyId: 3,
        cnpj: '333333333',
        stateRegistration: 'RN',
        companyName: 'Exemplo3',
        tradingName: 'Exemplo3',
        phone: '33333333',
        email: '333@hotmail.com',
      } as VsCompanyDetails,
    ];

    const result: Observable<VsCompaniesDetail> = of(companiesDatail);

    return result;
  }
}
