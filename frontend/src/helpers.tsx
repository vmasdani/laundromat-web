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
