import { Component } from '@angular/core';

@Component({
  selector: 'ore-information-sheet',
  templateUrl: './information-sheet.component.html',
  styleUrls: ["./information-sheet.component.css"]
})
export class InformationSheetComponent {

  public IntroText: string = "Welcome, this portal will allow you to consent to the Measuring Symmetry Preference by Age experiment and recieve the instructions to download the Virtual Reality Environment.";

}
