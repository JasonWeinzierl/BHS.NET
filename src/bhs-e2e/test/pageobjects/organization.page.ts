import Page from './page';

class OrganizationPage extends Page {
  get self() {
    return this.getByTestID('Organization');
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
