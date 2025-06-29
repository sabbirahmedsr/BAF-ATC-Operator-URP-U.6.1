
// path type = arrival:1, departure:2,  ground:3
// arrival path type = pre-arrival, arrivalApproach, land
// departure path type = take-off, post-departure 
// ground path type = taxi, parking,
// pre arrival

public enum APPathId {
    // none
    none,
    // 1-0-0-0 // arrival path //
    // 1-1-0-0 // arrival path // pre-arrival-path
    // 1-1-1-0 // arrival path // pre-arrival-path // 141-Degree
    avled_To_Tanap = 1110, tanap_To_VorCtg = 1111, vorCtg_To_Oneka = 1112, oneka_To_Admil = 1113, admil_To_Kandi = 1114, kandi_To_Vor = 1115,
    // 1-1-2-0 // arrival path // pre-arrival-path // 237-Degree
    bemak_To_VorJsr = 1120, vorJsr_To_Ikogu = 1121, ikogu_To_Akevo = 1122, akevo_To_14dme237 = 1123,
    // 1-2-0-0 // arrival path // arrival-approach-path // 
    // 1-2-1-0 // arrival path // arrival-approach-path // vor-ils
    entryToDME = 1210, dmeToVor = 1211, vor_To_Outbound342 = 1212, outbound342_ToLocalizer = 1213,
    // 1-2-2-0 // arrival path // arrival-approach-path // vor-arc-ils
    _14dme237_To_12dmeArc237 = 1220, _12dmeArc237_To_r300 = 1221, _r300_To_localizer = 1222,
    // 1-2-3-0 // arrival path // land-aproach-path 
    localizer_To_6DME = 130, _6DME_To_Land = 131, land_To_RwyEndPoint = 132
    //
    // 2-0-0-0 // departure path 
    // 2-1-0-0 // departure path // take off
    // 2-2-0-0 // departure path // post-departure
    //
    // 3-0-0-0 // ground path
    // 3-1-0-0 // ground path // taxi
    // 3-2-1-1 // ground path // parking
}
