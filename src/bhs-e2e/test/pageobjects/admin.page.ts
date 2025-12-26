import Page from './page';

class AdminPage extends Page {
  override open() {
    return super.open('/admin');
  }

  get self() {
    return this.getByTestID('AdminIndex');
  }

  get logoutLink() {
    return this.getByTestID('AdminIndex-Logout');
  }
}
export default new AdminPage();
