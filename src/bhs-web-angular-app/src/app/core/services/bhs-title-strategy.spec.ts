import { ChangeDetectionStrategy, Component } from '@angular/core';
import { TestBed } from '@angular/core/testing';
import { Title } from '@angular/platform-browser';
import { provideRouter, Router } from '@angular/router';
import { MockProvider } from 'ng-mocks';
import { BhsTitleStrategy } from './bhs-title-strategy';

@Component({ selector: 'app-fake', changeDetection: ChangeDetectionStrategy.OnPush })
class FakeComponent {}

describe('BhsTitleStrategy', () => {
  let service: BhsTitleStrategy;
  let router: Router;
  let title: Title;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [
        provideRouter([
          { path: 'foo', data: { title: 'foo title' }, component: FakeComponent },
          { path: 'bar', component: FakeComponent },
        ]),
        MockProvider(Title, {
          setTitle: jest.fn(),
        }),
      ],
    });
    service = TestBed.inject(BhsTitleStrategy);
    router = TestBed.inject(Router);
    title = TestBed.inject(Title);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should add a suffix if title is defined', async () => {
    const setTitle = jest.spyOn(title, 'setTitle');

    await router.navigate(['foo']);
    service.updateTitle(router.routerState.snapshot);

    expect(setTitle).toHaveBeenCalledWith('foo title | Belton Historical Society');
  });

  it('should use the app initial title if title is not defined', async () => {
    const setTitle = jest.spyOn(title, 'setTitle');

    await router.navigate(['bar']);
    service.updateTitle(router.routerState.snapshot);

    expect(setTitle).toHaveBeenCalledWith('Belton Historical Society');
  });
});
