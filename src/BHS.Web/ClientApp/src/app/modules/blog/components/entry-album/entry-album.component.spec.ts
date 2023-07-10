import { ComponentFixture, TestBed } from '@angular/core/testing';
import { EntryAlbumComponent } from './entry-album.component';

describe('EntryAlbumComponent', () => {
  let component: EntryAlbumComponent;
  let fixture: ComponentFixture<EntryAlbumComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ EntryAlbumComponent ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(EntryAlbumComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
