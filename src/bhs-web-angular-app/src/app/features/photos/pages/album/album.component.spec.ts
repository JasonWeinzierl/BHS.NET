import { AsyncPipe } from '@angular/common';
import { Directive } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ActivatedRoute, convertToParamMap, RouterLink } from '@angular/router';
import { EMPTY, of } from 'rxjs';
import { AlbumComponent } from './album.component';
import { PhotosService } from '@data/photos/services/photos.service';
import { SnippetPipe } from '@shared/pipes/snippet.pipe';
import { MockProvider } from 'ng-mocks';

@Directive({
  // eslint-disable-next-line @angular-eslint/directive-selector
  selector: '[ngSrc]',
})
class NgSrcStubComponent {}

describe('AlbumComponent', () => {
  let component: AlbumComponent;
  let fixture: ComponentFixture<AlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [
        AlbumComponent,
      ],
      providers: [
        MockProvider(PhotosService, {
          getAlbum$: () => EMPTY,
        }),
        MockProvider(ActivatedRoute, {
          paramMap: of(convertToParamMap({
            slug: '3',
          })),
        }),
      ],
    })
    .overrideComponent(AlbumComponent, {
      set: {
        imports: [
          RouterLink,
          NgSrcStubComponent,
          SnippetPipe,
          AsyncPipe,
        ],
      },
    })
    .compileComponents();

    fixture = TestBed.createComponent(AlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
