import { NgOptimizedImage } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { MockDirective } from 'ng-mocks';
import { EntryAlbumComponent } from './entry-album.component';

describe('EntryAlbumComponent', () => {
  let component: EntryAlbumComponent;
  let fixture: ComponentFixture<EntryAlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [
        EntryAlbumComponent,
        MockDirective(NgOptimizedImage),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EntryAlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render album', () => {
    fixture.componentRef.setInput('album', {
      slug: 'album-one',
      photos: [],
    });
    fixture.detectChanges();

    const element = fixture.nativeElement as HTMLElement;

    expect(element.textContent).toContain('This post contains photos:');
  });
});
