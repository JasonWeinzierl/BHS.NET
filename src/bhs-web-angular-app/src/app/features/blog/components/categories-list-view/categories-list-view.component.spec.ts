import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterLink, RouterModule } from '@angular/router';
import { CategoriesListViewComponent } from './categories-list-view.component';

describe('CategoriesListViewComponent', () => {
  let component: CategoriesListViewComponent;
  let fixture: ComponentFixture<CategoriesListViewComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        CategoriesListViewComponent,
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(CategoriesListViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should show loading', () => {
    expect(component.isLoading()).toBe(false); // off at first

    fixture.componentRef.setInput('isLoading', true);
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(component.isLoading()).toBe(true);
    expect(element.querySelector('.list-group .placeholder-glow')).toBeTruthy(); // should show placeholder
  });

  it('should show error', () => {
    expect(component.error()).toBeUndefined(); // no error at first

    fixture.componentRef.setInput('error', 'An error occurred.');
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(component.error()).toBeDefined();
    expect(element.querySelector('.list-group .list-group-item-danger')).toBeTruthy(); // should show danger banner
  });

  it('should show each category', () => {
    expect(component.categories()).toHaveLength(0); // empty at first

    fixture.componentRef.setInput('categories', [{
      slug: 'newsletters',
      name: 'Newsletters',
      postsCount: 1,
    }, {
      slug: 'stories',
      name: 'Stories',
      postsCount: 3,
    }]);
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(component.categories()).toHaveLength(2);
    expect(element.querySelector('.list-group')?.children).toHaveLength(2);

    const linkDebugElements = fixture.debugElement.queryAll(By.directive(RouterLink));
    const routerLinks = linkDebugElements.map(de => de.injector.get(RouterLink));

    expect(routerLinks).toHaveLength(2);
    expect(routerLinks[0].href).toBe('/apps/blog/category/newsletters');
    expect(routerLinks[1].href).toBe('/apps/blog/category/stories');
  });
});
