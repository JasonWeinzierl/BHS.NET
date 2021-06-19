import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { take } from 'rxjs/operators';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.scss']
})
export class NotFoundComponent implements OnInit {
  closestPath?: string;

  constructor(private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.route.data.pipe(take(1))
      .subscribe(data => {
        this.closestPath = data.closestPath;
      });
  }
}
