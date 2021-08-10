import { RouterModule, Routes } from "@angular/router";
import { ConsentComponent } from "./consent/consent.component";
import { ErrorComponent } from "./error/error.component";
import { InformationSheetComponent } from "./information-sheet/information-sheet.component";
import { InstructionsComponent } from "./instructions/instructions.component";
import { QuestionnaireComponent } from "./questionnaire/questionnaire.component";
import { RegisterComponent } from "./register/register.component";


const routes: Routes = [
    { path: 'download', component: RegisterComponent, pathMatch: 'full',
        children: [
            { path: 'information', component: InformationSheetComponent },
            { path: 'consent', component: ConsentComponent },
            { path: 'download', component: InstructionsComponent, data:{Type: "Install"} },
        ]
    },
    { path: 'complete', component: RegisterComponent, pathMatch: 'full',
        children: [
            { path: 'questionnaire', component: QuestionnaireComponent },
            { path: 'download', component: InstructionsComponent, data:{Type: "Uninstall"} },
        ]
    },
    {
        path: "",
        pathMatch: "full",
        redirectTo: "download"
    },
    {
        path: "error",
        pathMatch: "full",
        component: ErrorComponent
    }
  ];

  export const appRouting = RouterModule.forRoot(routes, { onSameUrlNavigation: 'reload', relativeLinkResolution: 'legacy' });