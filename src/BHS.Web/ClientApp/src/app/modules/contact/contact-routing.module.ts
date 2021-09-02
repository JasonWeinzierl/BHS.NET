import { RouterModule, Routes } from '@angular/router';
import { ContactComponent } from './page/contact.component';
import { NgModule } from '@angular/core';

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
