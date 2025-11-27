import { EditorAction } from './editor-action.enum';

export class EditorModalData <TData> {
  public action: EditorAction
  public data: TData

  constructor(action: EditorAction, data: TData) {
    this.action = action;
    this.data = data;
  }
}
