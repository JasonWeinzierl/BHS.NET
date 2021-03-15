import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { LocationComponent } from "./page/location.component";

const routes: Routes = [
    {
        path: '',
        component: LocationComponent,
        data: { title: 'Location' }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class LocationRoutingModule { }
