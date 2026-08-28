import Page from './page';

class OrganizationPage extends Page {
  override open() {
    return super.open('/about/by-laws-and-leadership');
  }

  get self() {
    return this.getByTestID('Organization');
  }

  get editButton() {
    return this.getByTestID('Organization-Leadership-Edit');
  }

  get officersLoading() {
    return this.getByTestID('Organization-Leadership-Officers-Loading');
  }

  get officersContainer() {
    return this.getByTestID('Organization-Leadership-Officers-Container');
  }

  get directorsLoading() {
    return this.getByTestID('Organization-Leadership-Directors-Loading');
  }

  get directorsContainer() {
    return this.getByTestID('Organization-Leadership-Directors-Container');
  }

  get bylawsContainer() {
    return this.getByTestID('Organization-Bylaws-Container');
  }
}
export default new OrganizationPage();
