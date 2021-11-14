using System;
using System.Collections.Generic;
using System.Text;
using Leviathan.Math;
using Silk.NET.GLFW;
using Silk.NET.OpenGL;
using OpenTK.Audio.OpenAL;
namespace Leviathan.Core.Windowing
{
    public unsafe struct NativeWindow
    {
        public String w_title;
        public Vector2d w_position;
        public Vector2d w_size;
        public bool w_hasfocus;
        public bool w_isIconified;

        public GraphicsContext graphics_context;
        public AudioContext audio_context;

        public unsafe WindowHandle* w_handle;
        public unsafe GlfwNativeWindow n_window;
        public IntPtr _hwinst;


        public static unsafe NativeWindow CreateWindow(uint width, uint height, string title, WindowMode mode)
        {
            NativeWindow output = new NativeWindow();
            output.graphics_context = GraphicsContext.Init();
            
            //output.graphics_context.glfw_context = Glfw.GetApi();
            //bool init = output.graphics_context.glfw_context.Init();
            //if (!init)
            //{
            //    throw new Exception("Error while creating GLFW context");
            //}


            output.graphics_context.glfw_context.WindowHint(WindowHintClientApi.ClientApi, ClientApi.OpenGL);
            output.graphics_context.glfw_context.WindowHint(WindowHintInt.ContextVersionMajor, 4);
            output.graphics_context.glfw_context.WindowHint(WindowHintInt.ContextVersionMinor, 3);
            output.graphics_context.glfw_context.WindowHint(WindowHintOpenGlProfile.OpenGlProfile, OpenGlProfile.Core);
            output.graphics_context.glfw_context.WindowHint(WindowHintBool.OpenGLForwardCompat, true);
            output.graphics_context.glfw_context.WindowHint(WindowHintBool.Resizable, true);
            output.graphics_context.glfw_context.WindowHint(WindowHintBool.DoubleBuffer, true);
            output.graphics_context.glfw_context.SetErrorCallback(OnGLFWError);

            switch (mode)
            {
                case WindowMode.FULLSCREEN:
                    CreateWindowFull(ref output, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED:
                    CreateWindowWindowed(ref output, title, (int)width, (int)height);
                    break;
                case WindowMode.WINDOWED_FULLSCREEN:
                    CreateWindowFullW(ref output, title, (int)width, (int)height);
                    break;
            }
            if (output.w_handle == null)
            {
                throw new Exception("Failed to create window");
            }

            output.graphics_context.InitOpenGL(ref output);
            string[] devices = AudioContext.EnumSoundDevices();
            output.audio_context = AudioContext.Init("");

            output.w_size = new Vector2d((int)width, (int)height);
            output.w_title = title;
            output.graphics_context.glfw_context.GetWindowPos(output.w_handle, out int x_pos, out int y_pos);
            output.w_position = new Vector2d(x_pos, y_pos);

            output.n_window = new GlfwNativeWindow(output.graphics_context.glfw_context, output.w_handle);
            if (output.n_window.Win32.HasValue)
            {
                output._hwinst = output.n_window.Win32.Value.Hwnd;
            }
            else
            {
                throw new Exception("Could not capture native window ptr");
            }

            

            return output;

        }

        private static unsafe void CreateWindowWindowed(ref NativeWindow wnd, String title, int width, int height)
        {
            wnd.w_handle = wnd.graphics_context.glfw_context.CreateWindow(width, height, title, null, null);
        }

        private static unsafe void CreateWindowFull(ref NativeWindow wnd, String title, int width, int height)
        {
            Monitor* mon = wnd.graphics_context.glfw_context.GetPrimaryMonitor();
            wnd.w_handle = wnd.graphics_context.glfw_context.CreateWindow(width, height, title, mon, null);
        }

        private static unsafe void CreateWindowFullW(ref NativeWindow wnd, String title, int width, int height)
        {
            Monitor* mon = wnd.graphics_context.glfw_context.GetPrimaryMonitor();
            VideoMode* vidmode = wnd.graphics_context.glfw_context.GetVideoMode(mon);

            wnd.graphics_context.glfw_context.WindowHint(WindowHintInt.RedBits, vidmode->RedBits);
            wnd.graphics_context.glfw_context.WindowHint(WindowHintInt.GreenBits, vidmode->GreenBits);
            wnd.graphics_context.glfw_context.WindowHint(WindowHintInt.BlueBits, vidmode->BlueBits);
            wnd.graphics_context.glfw_context.WindowHint(WindowHintInt.RefreshRate, vidmode->RefreshRate);
            wnd.w_handle = wnd.graphics_context.glfw_context.CreateWindow(width, height, title, mon, null);
            
        }

        public unsafe void SetDrawArea(uint x, uint y, uint width, uint height)
        {
            graphics_context.gl_context.Viewport((int)x, (int)y, width, height);
        }

        public unsafe void SwapBuffers()
        {
            graphics_context.glfw_context.SwapBuffers(w_handle);
        }

        public unsafe void ClearBuffer(ClearBufferMask mask)
        {
            graphics_context.gl_context.Clear((uint)mask);
        }

        public unsafe void PollEvents()
        {
            graphics_context.glfw_context.PollEvents();
        }

        public static unsafe void OnGLFWError(Silk.NET.GLFW.ErrorCode error, string description)
        {
            Console.WriteLine($"GLFW encountered an error: {description}");
        }

        public unsafe void Destroy()
        {
            graphics_context.Destroy();
            audio_context.Destroy();
        }

        


    }

    public class GraphicsContext : IDisposable
    {
        public Glfw glfw_context;
        public GL gl_context;

        private GraphicsContext()
        {
            glfw_context = null;
            gl_context = null;
        }

        public static unsafe GraphicsContext Init()
        {
            GraphicsContext output = new GraphicsContext();
            output.glfw_context = Glfw.GetApi();
            bool init = output.glfw_context.Init();
            if (!init)
            {
                throw new Exception("Error while creating GLFW context");
            }
            return output;
        }

        public unsafe void InitOpenGL(ref NativeWindow wnd)
        {
            glfw_context.ShowWindow(wnd.w_handle);
            glfw_context.MakeContextCurrent(wnd.w_handle);
            if (gl_context == null)
            {
                gl_context = GL.GetApi(glfw_context.GetProcAddress);
                if (gl_context == null)
                {
                    throw new Exception("Could not bind to GLFW PROC");
                }
            }
        }

        public void Destroy()
        {
            glfw_context.Terminate();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }

    public class AudioContext : IDisposable
    {
        public ALDevice sound_device;
        public ALContext context;

        private AudioContext()
        {
            sound_device = ALDevice.Null;
            context = ALContext.Null;
        }

        public static String[] EnumSoundDevices()
        {
            List<string> found = new List<string>(ALC.GetStringList(GetEnumerationStringList.DeviceSpecifier));
            return found.ToArray();
        }

        public static String[] EnumCaptureDevices()
        {
            List<string> found = new List<string>(ALC.GetStringList(GetEnumerationStringList.CaptureDeviceSpecifier));
            return found.ToArray();
        }

        public static unsafe AudioContext Init(string device_id)
        {
            AudioContext output = new AudioContext();
            output.sound_device = ALC.OpenDevice(device_id);
            if (output.sound_device.Handle == IntPtr.Zero)
            {
                throw new Exception("Could not create OpenAL audio device");
            }

            output.context = ALC.CreateContext(output.sound_device, (int*)null);
            if(output.context.Handle == IntPtr.Zero)
            {
                ALC.CloseDevice(output.sound_device);
                throw new Exception("Could not create OpenAL context");
            }
            
            bool succes = ALC.MakeContextCurrent(output.context);
            if(!succes)
            {
                ALC.DestroyContext(output.context);
                ALC.CloseDevice(output.sound_device);
                throw new Exception("Could not enable OpenAL context");
            }
            return output;
        }

        public void Destroy()
        {
            if(context != ALContext.Null && sound_device != ALDevice.Null)
            {
                ALC.DestroyContext(context);
                ALC.CloseDevice(sound_device);

                context.Handle = ALContext.Null;
                sound_device.Handle = ALDevice.Null;
            }
        }

        public void Dispose()
        {
            Destroy();
        }
    }
}
