import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { APP_PATHS } from '@app/app-paths';

export const resolveClosestPath: ResolveFn<string | undefined> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const typoPath = state.url.replace('/', '');
  const threshold = getThreshold(typoPath);
  const dictionary = Object.values(APP_PATHS)
    .filter(path => Math.abs(path.length - typoPath.length) < threshold);

  if (dictionary.length === 0) {
    return;
  }

  sortByDistance(typoPath, dictionary);

  return `/${dictionary[0]}`;
};

function getThreshold(path: string): number {
  return path.length < 5 ? 3 : 5;
}

function sortByDistance(typoPath: string, dictionary: Array<string>): void {
  const pathsDistance = {} as Record<string, number>;

  dictionary.sort((a, b) => {
    if (!(a in pathsDistance)) {
      pathsDistance[a] = levenshtein(a, typoPath);
    }
    if (!(b in pathsDistance)) {
      pathsDistance[b] = levenshtein(b, typoPath);
    }

    return pathsDistance[a] - pathsDistance[b];
  });
}

function levenshtein(a: string, b: string): number {
  if (a.length === 0) {
    return b.length;
  }
  if (b.length === 0) {
    return a.length;
  }

  const matrix = [];

  for (let index = 0; index <= b.length; index++) {
    matrix[index] = [index];
  }

  for (let index = 0; index <= a.length; index++) {
    matrix[0][index] = index;
  }

  for (let index = 1; index <= b.length; index++) {
    for (let index_ = 1; index_ <= a.length; index_++) {
      matrix[index][index_] = b.charAt(index - 1) === a.charAt(index_ - 1) ? matrix[index - 1][index_ - 1] : Math.min(
          matrix[index - 1][index_ - 1] + 1,
          matrix[index][index_ - 1] + 1,
          matrix[index - 1][index_] + 1,
        );
    }
  }

  return matrix[b.length][a.length];
}
