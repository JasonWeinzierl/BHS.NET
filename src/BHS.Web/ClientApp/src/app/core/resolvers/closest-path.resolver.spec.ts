import { ActivatedRoute, ActivatedRouteSnapshot, Router, RouterStateSnapshot } from '@angular/router';
import { resolveClosestPath } from './closest-path.resolver';
import { RouterTestingModule } from '@angular/router/testing';
import { TestBed } from '@angular/core/testing';

describe('resolveClosestPath', () => {
  let route: ActivatedRouteSnapshot;
  let state: RouterStateSnapshot;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [
        RouterTestingModule.withRoutes([]),
      ],
    });
    route = TestBed.inject(ActivatedRoute).snapshot;
    state = TestBed.inject(Router).routerState.snapshot;
  });

  it('should resolve', () => {
    state.url = 'hone';

    expect(resolveClosestPath(route, state)).toEqual('/home');
  });
});
