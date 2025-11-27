// navigation.service.ts
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { VsAuthorizationService, VsStorageService } from '@viasoft/common';
import { SessionService } from './session.service';
import { LAST_ROUTE_KEY, LAST_POLICY_KEY } from '../tokens';

@Injectable({
  providedIn: 'root'
})
export class NavigationService {
    private readonly chaveUltimaRota: string;
    private readonly chaveUltimaPolicy: string;
    private lastUrl: string | null = null;
    private redirectDone: boolean = false; 

  constructor(
    private vsAuthorizationService: VsAuthorizationService,
    private sessionService: SessionService,
    private vsStorage: VsStorageService,
    private router: Router){
      this.chaveUltimaRota =  LAST_ROUTE_KEY;
      this.chaveUltimaPolicy = LAST_POLICY_KEY;
  }

  private validarPermissaoRota(lastUrl: string, lastPolicy: string) {
    this.vsAuthorizationService.isGranted(lastPolicy)
        .then(permissionGranted => {
          if (!permissionGranted) {
            this.router.navigate(['/']);
            return;
          }
        })
        .catch(error => {
          console.error('Erro ao verificar permissão:', error);
          this.router.navigate([lastUrl]);
        });
  }

  private getUltimaRota(): string {
    return this.vsStorage.get(this.chaveUltimaRota) || '/';
  }

  private getUltimaPolicy(): string {
    return this.vsStorage.get(this.chaveUltimaPolicy);
  }

  public navegarUltimaRotaAcessada() {
    if (this.redirectDone) {
      return;
    }
    this.redirectDone = true;
    const lastUrl = this.getUltimaRota();
    const lastPolicy = this.getUltimaPolicy();

    if (lastPolicy) {
      this.validarPermissaoRota(lastUrl, lastPolicy);
    }

    if (this.sessionService.currentCompany){
      this.router.navigate([lastUrl]);
    }
  }

  public setRedirectDone(value: boolean) {
    this.redirectDone = value;
  }

  public setLastUrl(url: string): void {
    this.lastUrl = url;
    this.redirectDone = false; 
  }

  public getLastUrl(): string | null {
    return this.lastUrl;
  }

  public redirectToLastUrl(): void {
    if (!this.redirectDone && this.lastUrl) {
      this.router.navigateByUrl(this.lastUrl);
      this.redirectDone = true; 
    }
  }
}
