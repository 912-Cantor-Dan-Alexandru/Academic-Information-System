import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from './Models/material/material.module';
import { ReactiveFormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';
import { StudentMainPageComponent } from './Pages/student-main-page/student-main-page.component';
import { TeacherMainPageComponent } from './Pages/teacher-main-page/teacher-main-page.component';
import { StaffMainPageComponent } from './Pages/staff-main-page/staff-main-page.component';
import { HomepageComponent } from './Pages/homepage/homepage.component'

@NgModule({
  declarations: [
    AppComponent,
    StudentMainPageComponent,
    TeacherMainPageComponent,
    StaffMainPageComponent,
    HomepageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    MaterialModule,
    ReactiveFormsModule,
    HttpClientModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
