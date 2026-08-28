import { TestBed } from '@angular/core/testing';
import { AuthService } from '@auth0/auth0-angular';
import { MockProvider } from 'ng-mocks';
import { firstValueFrom, of, throwError } from 'rxjs';
import { PermissionsService } from './permissions.service';

const ACCESS_TOKEN = `header.${btoa(JSON.stringify({ permissions: ['write:leadership'] }))}.signature`;

describe('PermissionsService', () => {
  it('should find a permission in an authenticated user token', async () => {
    TestBed.configureTestingModule({
      providers: [
        MockProvider(AuthService, {
          isAuthenticated$: of(true),
          getAccessTokenSilently: ((options) => options?.detailedResponse
            ? throwError(() => new Error('detailedResponse not implemented'))
            : of(ACCESS_TOKEN)) as AuthService['getAccessTokenSilently'],
        }),
      ],
    });

    const service = TestBed.inject(PermissionsService);
    const hasPermission = await firstValueFrom(service.hasPermission$('write:leadership'), { defaultValue: false });

    expect(hasPermission).toBe(true);
  });

  it('should deny permissions to unauthenticated users', async () => {
    TestBed.configureTestingModule({
      providers: [
        MockProvider(AuthService, {
          isAuthenticated$: of(false),
        }),
      ],
    });

    const service = TestBed.inject(PermissionsService);
    const hasPermission = await firstValueFrom(service.hasPermission$('write:leadership'), { defaultValue: true });

    expect(hasPermission).toBe(false);
  });
});
