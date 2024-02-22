import { ActivatedRouteSnapshot, ResolveFn, RouterStateSnapshot } from '@angular/router';
import { paths } from '@app/app-paths';

export const resolveClosestPath: ResolveFn<string | null> = (
  route: ActivatedRouteSnapshot,
  state: RouterStateSnapshot,
) => {
  const typoPath = state.url.replace('/', '');
  const threshold = getThreshold(typoPath);
  const dictionary = Object.values(paths)
    .filter(path => Math.abs(path.length - typoPath.length) < threshold);

  if (!dictionary.length) {
    return null;
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

  for (let i = 0; i <= b.length; i++) {
    matrix[i] = [i];
  }

  for (let j = 0; j <= a.length; j++) {
    matrix[0][j] = j;
  }

  for (let i = 1; i <= b.length; i++) {
    for (let j = 1; j <= a.length; j++) {
      if (b.charAt(i - 1) === a.charAt(j - 1)) {
        matrix[i][j] = matrix[i - 1][j - 1];
      } else {
        matrix[i][j] = Math.min(
          matrix[i - 1][j - 1] + 1,
          matrix[i][j - 1] + 1,
          matrix[i - 1][j] + 1,
        );
      }
    }
  }

  return matrix[b.length][a.length];
}
