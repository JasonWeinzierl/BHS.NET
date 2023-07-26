import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ContactComponent } from './pages/contact.component';

const routes: Routes = [
    {
        path: '',
        component: ContactComponent,
        data: { title: 'Contact Us' },
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ContactRoutingModule { }
