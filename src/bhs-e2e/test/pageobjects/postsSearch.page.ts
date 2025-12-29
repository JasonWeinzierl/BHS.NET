import Page from './page';

class PostsSearchPage extends Page {
  get self() {
    return this.getByTestID('PostsSearch');
  }

  get searchInput() {
    return this.getByTestID('PostsSearch-SearchInput');
  }

  get searchButton() {
    return this.getByTestID('PostsSearch-SearchButton');
  }
}
export default new PostsSearchPage();
