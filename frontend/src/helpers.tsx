export const formatDateTimeShort = (s?: string | null | undefined) => {
  try {
    return Intl.DateTimeFormat("en-US", {
      dateStyle: "short",
      timeStyle: "short",
    }).format(new Date(s ?? ""));
  } catch (e) {
    return "Invalid date";
  }
};

export type LaundryRecordStatus =
  | "ONGOING"
  | "DONE"
  | "PROCESSING"
  | "PICKED_UP"
  | "DISGUARDED";
export const laundryRecordStatuses: LaundryRecordStatus[] = [
  "PROCESSING",
  "PICKED_UP",
  "DONE",
  "DISGUARDED",
];
