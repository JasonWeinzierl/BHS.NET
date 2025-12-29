import Page from './page';

class PhotosPage extends Page {
  override open() {
    return super.open('/apps/photos');
  }

  get self() {
    return this.getByTestID('PhotosIndex');
  }

  get title() {
    return this.getByTestID('PhotosIndex-Title');
  }

  get albums() {
    return this.getByTestID('PhotosIndex-Albums');
  }

  get loading() {
    return this.getByTestID('PhotosIndex-Loading');
  }

  get error() {
    return this.getByTestID('PhotosIndex-Error');
  }
}
export default new PhotosPage();
