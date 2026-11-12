mergeInto(LibraryManager.library, {
  $FitCheckPointer: {
    wanted: false,
    pending: false,
    installed: false,
    locked: false,
    releasePending: false,
    lastRelease: -1000,
    fullscreen: false,
    intentionalFullscreenExit: false,
    released: function () {
      var now = performance.now();
      if (now - FitCheckPointer.lastRelease < 160) return;
      FitCheckPointer.lastRelease = now;
      FitCheckPointer.releasePending = true;
    },
    acquire: function () {
      var canvas = Module.canvas;
      if (!FitCheckPointer.wanted || FitCheckPointer.pending ||
          document.pointerLockElement === canvas || !canvas.isConnected) return;
      // A denied request is recoverable. Wait for one click, never queue a
      // Unity/Emscripten request that consumes the NEXT gesture as well.
      if (navigator.userActivation && !navigator.userActivation.isActive) return;
      FitCheckPointer.pending = true;
      try {
        var result = canvas.requestPointerLock();
        if (result && result.then) result.then(function () {
          FitCheckPointer.pending = false;
        }, function () { FitCheckPointer.pending = false; });
        else FitCheckPointer.pending = false;
      } catch (error) { FitCheckPointer.pending = false; }
    },
    install: function () {
      if (FitCheckPointer.installed) return;
      FitCheckPointer.installed = true;
      document.addEventListener('pointerlockchange', function () {
        var locked = document.pointerLockElement === Module.canvas;
        // Browser Escape can consume keydown before Unity sees it. An external
        // unlock only frees the camera cursor; menu releases set wanted=false first.
        if (FitCheckPointer.locked && !locked && FitCheckPointer.wanted) {
          FitCheckPointer.wanted = false;
          FitCheckPointer.released();
        }
        FitCheckPointer.locked = locked;
      });
      document.addEventListener('fullscreenchange', function () {
        var fullscreen = !!document.fullscreenElement;
        if (FitCheckPointer.fullscreen && !fullscreen && !FitCheckPointer.intentionalFullscreenExit) FitCheckPointer.released();
        FitCheckPointer.fullscreen = fullscreen;
        FitCheckPointer.intentionalFullscreenExit = false;
      });
      Module.canvas.addEventListener('pointerdown', function (event) {
        if (event.button !== 0) return;
        Module.canvas.focus();
        FitCheckPointer.acquire();
      }, true);
    }
  },
  FitCheck_SetPointerCapture__deps: ['$FitCheckPointer'],
  FitCheck_SetPointerCapture: function (wanted) {
    FitCheckPointer.install();
    var changed = FitCheckPointer.wanted !== !!wanted;
    FitCheckPointer.wanted = !!wanted;
    if (!wanted) {
      Module.canvas.style.cursor = 'default';
      if (document.pointerLockElement === Module.canvas) document.exitPointerLock();
    } else {
      Module.canvas.style.cursor = 'none';
      // A UI button's mouse-up still has transient browser activation here.
      // Request directly, instead of deferring until a second canvas click.
      if (changed) FitCheckPointer.acquire();
    }
  },
  FitCheck_HasPointerCapture: function () {
    return document.pointerLockElement === Module.canvas ? 1 : 0;
  },
  FitCheck_TakeRelease__deps: ['$FitCheckPointer'],
  FitCheck_TakeRelease: function () {
    var pending = FitCheckPointer.releasePending;
    FitCheckPointer.releasePending = false;
    return pending ? 1 : 0;
  },
  FitCheck_EnterFullscreen__deps: ['$FitCheckPointer'],
  FitCheck_EnterFullscreen: function () {
    FitCheckPointer.install();
    if (document.fullscreenElement || !Module.canvas.requestFullscreen) return;
    try {
      Module.canvas.requestFullscreen({navigationUI: 'hide'}).catch(function () {});
    } catch (error) {}
  },
  FitCheck_HasFullscreen: function () { return document.fullscreenElement ? 1 : 0; },
  FitCheck_ExitFullscreen__deps: ['$FitCheckPointer'],
  FitCheck_ExitFullscreen: function () {
    FitCheckPointer.intentionalFullscreenExit = true;
    FitCheckPointer.wanted = false;
    FitCheckPointer.releasePending = false;
    if (document.pointerLockElement === Module.canvas) document.exitPointerLock();
    // A browser player cannot close the host tab. Leave fullscreen without
    // quitting Unity, navigating or reloading. An already-inline player stays intact.
    try {
      var exit = document.exitFullscreen || document.webkitExitFullscreen;
      if (exit) { var pending = exit.call(document); if (pending && pending.catch) pending.catch(function () {}); }
    } catch (error) {}
  }
});
