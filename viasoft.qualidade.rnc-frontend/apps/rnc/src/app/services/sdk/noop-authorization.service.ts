import { Injectable } from '@angular/core';
import { VsAuthorizationService, VsAuthorizationStorageService } from '@viasoft/common';

@Injectable()
export class NoopAuthorizationService extends VsAuthorizationService {
  constructor(storageService: VsAuthorizationStorageService) {
    super(storageService);
  }

  public async isGranted(
    permissions: string | string[],
    operation: 'OR' | 'AND' = 'AND'
  ): Promise<boolean> {
    permissions = typeof permissions === 'string' ? [permissions] : permissions;
    return this.storageService.checkUserPermissions(permissions, operation);
  }

  public async isGrantedMap(permissions: string[]): Promise<Map<string, boolean>> {
    return this.storageService.checkUserPermissionsMap(permissions);
  }
}
