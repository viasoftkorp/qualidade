import { ChangeDetectorRef, Component } from "@angular/core";
import { FormControl, FormGroup, FormsModule } from "@angular/forms";
import { MatTooltipModule } from "@angular/material/tooltip";
import { TranslateService } from "@ngx-translate/core";
import { VsCommonModule, VsSubscriptionManager } from "@viasoft/common";
import { getValueFromObjectByName, SimplifiedIMask, VS_BASE_DATEPICKER_CONFIG, VS_DATE_MASK_IMASK_MAP, VS_DATETIME_MASK_IMASK_MAP, VsDatepickerModule, VsLabelModule, VsTableCellCustomComponent } from "@viasoft/components";

@Component({
    selector: 'right-side-datepicker-grid-cell',
    templateUrl: './right-side-datepicker-grid-cell.component.html',
    standalone: true,
    imports: [FormsModule, VsLabelModule, VsDatepickerModule, VsCommonModule, MatTooltipModule],
})
export class RightSideDatepickerGridCellComponent extends VsTableCellCustomComponent<any, string> {
    private subs = new VsSubscriptionManager();
    public text: string;
    public mask: string | SimplifiedIMask;
    public datepickerMode: 'day' | 'daytime' = undefined;
    public datepickerFormGroup: FormGroup<{ [key: string]: FormControl<any>; }>;
    public datepickerConfig = { ...VS_BASE_DATEPICKER_CONFIG, opens: 'right' }

    constructor(private cdr: ChangeDetectorRef, private translateService: TranslateService) {
        super();
    }

    ngOnDestroy(): void {
        this.subs.clear();
    }

    public afterFillInputs(): void {
        let formattedValue = this.getFormattedValue();
        this.normalizeMask();
        this.text = this.shouldTranslate ? this.translateService.instant(formattedValue) : formattedValue;
    }

    public refresh(): void {
        this.normalizeMask();
        this.cdr.markForCheck();
    }

    private getFormattedValue(): string {
        let value = getValueFromObjectByName(this.data, this.fieldName);
        try {
            return this.format ? this.format(value) : value;
        } catch (error) {
            console.error(error);
            return value;
        }
    }

    private normalizeMask(): void {
        if (!this.isInEditMode || !this.editRowOptions?.mask) {
            return;
        }
        this.mask = this.editRowOptions.mask;
        const isDateMask = typeof this.editRowOptions.mask === 'string'
            && [VS_DATE_MASK_IMASK_MAP.name, VS_DATETIME_MASK_IMASK_MAP.name].includes(this.editRowOptions.mask);
        if (isDateMask) {
            this.datepickerMode = this.editRowOptions.mask === VS_DATE_MASK_IMASK_MAP.name ? 'day' : 'daytime';
            this.datepickerFormGroup = new FormGroup({
                [this.fieldName]: new FormControl(this.data[this.fieldName])
            });
            this.subs.add('date-changed', this.datepickerFormGroup.get(this.fieldName).valueChanges.subscribe((newValue) => {
                this.data[this.fieldName] = newValue;
            }));
        }
    }
}