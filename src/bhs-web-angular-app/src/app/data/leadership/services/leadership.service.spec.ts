import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { TestBed } from '@angular/core/testing';
import { firstValueFrom } from 'rxjs';
import { LeadershipService } from './leadership.service';

describe('LeadershipService', () => {
  let service: LeadershipService;
  let httpTesting: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [],
      providers: [
        provideHttpClient(withInterceptorsFromDi()),
        provideHttpClientTesting(),
      ],
    });
    service = TestBed.inject(LeadershipService);
    httpTesting = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpTesting.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get offices', async () => {
    const offices = [{ title: 'President', sortOrder: 1 }];
    const resultPromise = firstValueFrom(service.getOffices$(), { defaultValue: [] });

    const request = httpTesting.expectOne('/api/leadership/offices');

    expect(request.request.method).toBe('GET');

    request.flush(offices);

    await expect(resultPromise).resolves.toEqual(offices);
  });

  it('should update officers', async () => {
    const dateStarted = new Date('2025-01-01T00:00:00Z');
    const officers = [{ title: 'President', name: 'Jane Doe', dateStarted }];
    const resultPromise = firstValueFrom(service.updateOfficers$(officers), { defaultValue: [] });

    const request = httpTesting.expectOne('/api/leadership/officers');

    expect(request.request.method).toBe('PUT');

    expect(request.request.body).toEqual(officers);

    request.flush([{ title: 'President', name: 'Jane Doe', dateStarted: dateStarted.toISOString() }]);

    await expect(resultPromise).resolves.toEqual(officers);
  });

  it('should add directors', async () => {
    const directors = [{ name: 'Jane Doe', year: 2026 }];
    const resultPromise = firstValueFrom(service.addDirectors$(directors), { defaultValue: [] });

    const request = httpTesting.expectOne('/api/leadership/directors');

    expect(request.request.method).toBe('POST');

    expect(request.request.body).toEqual(directors);

    request.flush([{ name: 'Jane Doe', year: '2026' }]);

    await expect(resultPromise).resolves.toEqual([{ name: 'Jane Doe', year: '2026' }]);
  });

  it('should delete a director', async () => {
    const resultPromise = firstValueFrom(service.deleteDirector$({ name: 'Jane Doe Jr.', year: 2026 }), { defaultValue: undefined });

    const request = httpTesting.expectOne('/api/leadership/directors/2026/Jane%20Doe%20Jr.');

    expect(request.request.method).toBe('DELETE');

    request.flush('', { status: 204, statusText: 'No Content' });

    await expect(resultPromise).resolves.toBe('');
  });
});
