import { NgOptimizedImage } from '@angular/common';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { RouterLink , RouterModule } from '@angular/router';
import { MockDirective } from 'ng-mocks';
import { EntryAlbumComponent } from './entry-album.component';

describe('EntryAlbumComponent', () => {
  let component: EntryAlbumComponent;
  let fixture: ComponentFixture<EntryAlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        RouterModule.forRoot([]),
        EntryAlbumComponent,
        MockDirective(NgOptimizedImage),
      ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EntryAlbumComponent);
    component = fixture.componentInstance;
    component.album = {
      slug: 'album-one',
      photos: [{
        id: 'photo-one',
        imagePath: 'http://example.com/photo-one.jpg',
        datePosted: new Date(),
      }],
    };
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should render links to each photo', () => {
    const links = fixture.debugElement.queryAll(By.directive(RouterLink));

    expect(links).toHaveLength(1);
  });
});
