import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { ContactComponent } from './page/contact.component';

const routes: Routes = [
    {
        path: '',
        component: ContactComponent,
        data: { title: 'Contact Us' }
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule]
})
export class ContactRoutingModule { }
