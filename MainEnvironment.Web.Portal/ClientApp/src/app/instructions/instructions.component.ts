import { Component } from '@angular/core';

@Component({
  selector: 'ore-instructions',
  templateUrl: './instructions.component.html',
  styleUrls: ["./instructions.component.css"]
})
export class InstructionsComponent {

  public IntroText: string = "Welcome, this portal will allow you to consent to the Measuring Symmetry Preference by Age experiment and recieve the instructions to download the Virtual Reality Environment.";

}
