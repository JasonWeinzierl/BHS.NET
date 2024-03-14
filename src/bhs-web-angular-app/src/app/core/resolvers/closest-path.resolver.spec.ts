import { TestBed } from '@angular/core/testing';
import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterModule , RouterStateSnapshot } from '@angular/router';
import { resolveClosestPath } from './closest-path.resolver';

describe('resolveClosestPath', () => {
  let route: ActivatedRouteSnapshot;
  let state: RouterStateSnapshot;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
      ],
    });
    route = TestBed.inject(ActivatedRoute).snapshot;
    state = TestBed.inject(Router).routerState.snapshot;
  });

  it('should resolve', () => {
    state.url = 'hone';

    expect(resolveClosestPath(route, state)).toBe('/home');
  });
});
