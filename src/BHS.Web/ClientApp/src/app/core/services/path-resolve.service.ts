import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from '@angular/router';
import { Injectable } from '@angular/core';
import { paths } from '@app/app-paths';

@Injectable({
  providedIn: 'root'
})
export class PathResolveService implements Resolve<string | null> {

  resolve(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): string | null {
    const typoPath = state.url.replace('/', '');
    const threshold = this.getThreshold(typoPath);
    const dictionary = Object.values(paths)
      .filter(path => Math.abs(path.length - typoPath.length) < threshold);

    if (!dictionary.length) {
      return null;
    }

    this.sortByDistance(typoPath, dictionary);

    return `/${dictionary[0]}`;
  }

  private getThreshold(path: string): number {
    return path.length < 5 ? 3 : 5;
  }

  private sortByDistance(typoPath: string, dictionary: string[]): void {
    const pathsDistance = {} as { [name: string]: number };

    dictionary.sort((a, b) => {
      if (!(a in pathsDistance)) {
        pathsDistance[a] = this.levenshtein(a, typoPath);
      }
      if (!(b in pathsDistance)) {
        pathsDistance[b] = this.levenshtein(b, typoPath);
      }

      return pathsDistance[a] - pathsDistance[b];
    });
  }

  private levenshtein(a: string, b: string): number {
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
            matrix[i - 1][j] + 1
          );
        }
      }
    }

    return matrix[b.length][a.length];
  }
}
