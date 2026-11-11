"""Serve only this Web build on localhost; supports Unity's compressed fallback."""
import argparse
from functools import partial
from http.server import SimpleHTTPRequestHandler, ThreadingHTTPServer
from pathlib import Path
import webbrowser

class Handler(SimpleHTTPRequestHandler):
    extensions_map={**SimpleHTTPRequestHandler.extensions_map,'.wasm':'application/wasm','.unityweb':'application/octet-stream','.data':'application/octet-stream'}
    def end_headers(self):
        self.send_header('Cache-Control','no-cache')
        super().end_headers()

def main():
    parser=argparse.ArgumentParser(description='Fit Check — servidor local da versão Web')
    parser.add_argument('--port',type=int,default=8710)
    parser.add_argument('--no-browser',action='store_true')
    args=parser.parse_args()
    root=Path(__file__).resolve().parent
    if not (root/'index.html').is_file():parser.error('Coloque serve.py junto do index.html da build.')
    url=f'http://127.0.0.1:{args.port}/'
    server=ThreadingHTTPServer(('127.0.0.1',args.port),partial(Handler,directory=str(root)))
    print('Fit Check Web: '+url,flush=True)
    print('Mantenha esta janela aberta enquanto joga. Ctrl+C encerra o servidor.',flush=True)
    if not args.no_browser:webbrowser.open(url)
    try:server.serve_forever()
    except KeyboardInterrupt:pass
    finally:server.server_close()

if __name__=='__main__':main()
