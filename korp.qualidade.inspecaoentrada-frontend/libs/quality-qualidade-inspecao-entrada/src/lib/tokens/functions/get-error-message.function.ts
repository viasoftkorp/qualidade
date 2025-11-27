import { HttpErrorResponse } from '@angular/common/http';

export function getErrorMessage(httpError: HttpErrorResponse): string {
  return httpError?.error?.Message || 'Um erro desconhecido ocorreu.';
}
