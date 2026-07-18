# VR Disaster Preparedness Simulation — Project Scaffold

Generated for Group V-10, based on the project proposal (household fire
evacuation VR trainer). Built for **Unity 6 + XR Interaction Toolkit (XRI)**.

This is a code + folder scaffold, not a finished playable build — you still
need to import XRI, build the level geometry, and wire references in the
Inspector. Everything below tells you exactly how.

## 1. Import into Unity 6

1. Create a new **3D (URP)** project in Unity 6 (URP recommended for VR
   performance; XRI samples assume it).
2. Copy the `Assets/` folder from this scaffold into your project's `Assets/`
   folder (merge, don't overwrite).
3. Window → Package Manager → install:
   - **XR Interaction Toolkit** (com.unity.xr.interaction.toolkit)
   - **XR Plugin Management** → enable your headset's provider (OpenXR
     recommended for Quest/generic PCVR)
   - **TextMeshPro** (Unity will prompt "Import TMP Essentials" — do it, the
     UI scripts use `TMP_Text`)
4. In the XRI package, import the **Starter Assets** sample — this gives you
   a ready-made `XR Origin (Action-based)` prefab with hand/controller input
   already bound. Drag that prefab into your scene as your player.

## 2. Tag & Layer setup

- Add a **"Player"** tag (Edit → Project Settings → Tags and Layers).
- Add an **`XRPlayerSetup`** component (from
  `Assets/Scripts/Player/XRPlayerSetup.cs`) to the root of the XR Origin
  prefab. It auto-tags the rig and its colliders as "Player" so the trigger
  scripts below detect the user.
- Make sure the XR Origin has a `CharacterController` or capsule `Collider`
  (the Starter Assets prefab already includes one).

## 3. Scene structure (maps to the proposal's 6-step workflow)

| Proposal step | Scaffold piece |
|---|---|
| 1. User starts the simulation | `GameManager` in `Idle` state, `FireEventTrigger` armed |
| 2. A fire emergency is triggered | `FireEventTrigger.Fire()` → `GameManager.TriggerEmergency()` |
| 3. Smoke and alarm effects appear | `AlarmController.ActivateAlarm()`, `SmokeZone.SetActiveSmoke(true)` |
| 4. User follows exit signs and avoids hazards | `ExitSign` (lights up on emergency), `HazardZone` (registers mistakes) |
| 5. User reaches the assembly point safely | `AssemblyPoint` trigger → `GameManager.CompleteEvacuation()` |
| 6. System displays performance results | `ResultsPanelController` shows time + mistakes from `EvacuationResult` |

### Recommended empty GameObjects in each scene

- `GameManager` — add `GameManager`, `EvacuationTimer`, `MistakeTracker`
  components (all three live on the same object).
- `FireTrigger` — place at the point/room where the fire should start; add
  `FireEventTrigger`, set its `Trigger Mode` (Timed / PlayerEntersVolume /
  Manual) and drag in your `AlarmController` + `SmokeZone[]` references.
- One `AlarmController` per alarm prop (ceiling siren + strobe light).
- One `SmokeZone` per corridor/room that fills with smoke (add a trigger
  `BoxCollider` sized to the room + a `ParticleSystem` for visual smoke).
- One `HazardZone` per hazard (blocked door, exposed wiring, fire itself) —
  set `Severity` to `MinorMistake` or `CriticalFailure`.
- One `ExitSign` per illuminated exit sign prop, placed along the correct
  route (`Is On Correct Route = true`) and optionally along decoy routes
  (`false`) if you want to penalize wrong turns later.
- One `AssemblyPoint` trigger volume outside the building.

### UI Canvas

- **HUD**: world-space canvas (e.g. parented to the player's wrist or a
  fixed HUD anchor) with `HUDController`, two `TMP_Text` fields (timer,
  mistakes), wired to the `GameManager` / `EvacuationTimer` / `MistakeTracker`
  in the scene.
- **Results panel**: separate canvas, inactive by default, with
  `ResultsPanelController` + `TMP_Text` fields for time/mistakes/verdict and
  a Retry `Button`.

## 4. Wiring checklist in the Inspector

1. `FireTrigger` → drag `AlarmController` and `SmokeZone` objects into its
   arrays.
2. `HUDController` / `ResultsPanelController` → drag the `GameManager`
   object (and `EvacuationTimer` / `MistakeTracker` for the HUD) into their
   fields.
3. Confirm every trigger collider (`SmokeZone`, `HazardZone`,
   `AssemblyPoint`, `ExitSign`) has **Is Trigger** checked — the `Reset()`
   methods do this automatically the first time you add the component, but
   double-check after resizing colliders.

## 5. Test loop

1. Enter Play Mode with headset connected (or use XRI's Device Simulator for
   desktop testing without a headset).
2. Confirm the fire trigger fires (console log / alarm sound).
3. Walk into a `SmokeZone` and a `HazardZone` — check the Console for
   `[MistakeTracker] Mistake #...` logs.
4. Reach the `AssemblyPoint` — results panel should appear with elapsed time
   and mistake count.

## 6. Next steps not covered by this scaffold

- 3D level art (rooms, corridors, furniture, fire/smoke VFX assets).
- Locomotion comfort options (vignette on turn, teleport vs. smooth move) —
  configure via the XRI Starter Assets prefab's `Locomotion` components.
- Audio assets (alarm sound, ambient fire crackle).
- A main menu / "Start Drill" scene before the simulation scene.

## Folder structure

```
Assets/
  Scripts/
    Core/        GameManager, EvacuationTimer, MistakeTracker
    Emergency/   FireEventTrigger, AlarmController, SmokeZone, HazardZone
    Navigation/  ExitSign, AssemblyPoint
    Player/      XRPlayerSetup
    UI/          HUDController, ResultsPanelController
  Scenes/        (add your .unity scene here)
  Prefabs/       (add XR Origin, alarm, hazard prefabs here)
  Materials/
  Audio/
```
