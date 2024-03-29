import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LocationComponent } from './pages/location.component';

const routes: Routes = [
    {
        path: '',
        component: LocationComponent,
        data: { title: 'Location' },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class LocationRoutingModule { }
