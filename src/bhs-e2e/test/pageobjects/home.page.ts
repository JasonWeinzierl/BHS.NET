import Page from './page';

class HomePage extends Page {
  override open() {
    return super.open('/');
  }

  get self() {
    return this.getByTestID('Home');
  }

  get title() {
    return this.getByTestID('Home-Title');
  }

  get carousel() {
    return this.getByTestID('Home-Carousel');
  }
}
export default new HomePage();
