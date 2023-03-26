import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { MainComponent } from './main/main.component';
import { SecondPageComponent } from './second-page/second-page.component';

const routes: Routes = [{path:'', component: MainComponent},
{path:'second-page', component: SecondPageComponent}];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
