import { RouterModule, Routes } from '@angular/router';
import { LocationComponent } from './page/location.component';
import { NgModule } from '@angular/core';

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
