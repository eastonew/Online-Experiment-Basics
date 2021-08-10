import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { appRouting } from './app.routes';
import { ConsentComponent } from './consent/consent.component';
import { ErrorComponent } from './error/error.component';
import { InformationSheetComponent } from './information-sheet/information-sheet.component';
import { InstructionsComponent } from './instructions/instructions.component';
import { QuestionnaireComponent } from './questionnaire/questionnaire.component';
import { RegisterComponent } from './register/register.component';

@NgModule({
  declarations: [
    AppComponent,
    ConsentComponent,
    ErrorComponent,
    InformationSheetComponent,
    InstructionsComponent,
    QuestionnaireComponent,
    RegisterComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    appRouting
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
